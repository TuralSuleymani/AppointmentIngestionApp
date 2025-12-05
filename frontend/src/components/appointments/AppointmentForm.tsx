import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { appointmentSchema, type AppointmentFormData } from '../../schemas/appointmentSchema';
import { useCreateAppointment } from '../../hooks/useAppointments';
import { Button } from '../common/Button';
import type { AppointmentRequestDto } from '../../types/appointment';

interface AppointmentFormProps {
  onSuccess: () => void;
  onCancel: () => void;
}

export const AppointmentForm: React.FC<AppointmentFormProps> = ({ onSuccess, onCancel }) => {
  const { createAppointment, loading, error: apiError } = useCreateAppointment();
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<AppointmentFormData>({
    resolver: zodResolver(appointmentSchema),
  });

  const onSubmit = async (data: AppointmentFormData) => {
    console.log('Form data received:', data);
    
    // Validate appointmentDate is not empty
    if (!data.appointmentDate || data.appointmentDate.trim() === '') {
      console.error('Appointment date is empty');
      return;
    }

    try {
      // Convert datetime-local format to ISO 8601 format for .NET
      const appointmentDate = new Date(data.appointmentDate);
      const isoDate = appointmentDate.toISOString();
      
      console.log('Original date:', data.appointmentDate);
      console.log('Converted to ISO:', isoDate);

      const payload: AppointmentRequestDto = {
        clientName: data.clientName.trim(),
        appointmentDate: isoDate,
      };

      // Only add serviceDurationMinutes if it has a valid value
      if (data.serviceDurationMinutes && data.serviceDurationMinutes !== '') {
        const duration = Number(data.serviceDurationMinutes);
        if (!isNaN(duration) && duration > 0) {
          payload.serviceDurationMinutes = duration;
        }
      }

      console.log('Payload being sent:', payload);
      
      const result = await createAppointment(payload);
      setSuccessMessage(result.message);
      reset();
      setTimeout(() => {
        onSuccess();
      }, 1500);
    } catch (err) {
      console.error('Form submission error:', err);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4" noValidate>
      {successMessage && (
        <div className="p-4 bg-green-50 border border-green-200 rounded-md">
          <p className="text-sm text-green-800">{successMessage}</p>
        </div>
      )}

      {apiError && (
        <div className="p-4 bg-red-50 border border-red-200 rounded-md">
          <p className="text-sm text-red-800">{apiError}</p>
        </div>
      )}

      <div className="mb-4">
        <label htmlFor="clientName" className="block text-sm font-medium text-gray-700 mb-1">
          Client Name <span className="text-red-500">*</span>
        </label>
        <input
          id="clientName"
          type="text"
          {...register('clientName')}
          className={`w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${
            errors.clientName ? 'border-red-500' : 'border-gray-300'
          }`}
          placeholder="Enter client name"
        />
        {errors.clientName && (
          <p className="mt-1 text-sm text-red-600">{errors.clientName.message}</p>
        )}
      </div>

      <div className="mb-4">
        <label htmlFor="appointmentDate" className="block text-sm font-medium text-gray-700 mb-1">
          Appointment Date <span className="text-red-500">*</span>
        </label>
        <input
          id="appointmentDate"
          type="datetime-local"
          {...register('appointmentDate')}
          className={`w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${
            errors.appointmentDate ? 'border-red-500' : 'border-gray-300'
          }`}
          required
        />
        {errors.appointmentDate && (
          <p className="mt-1 text-sm text-red-600">{errors.appointmentDate.message}</p>
        )}
      </div>

      <div className="mb-4">
        <label htmlFor="serviceDurationMinutes" className="block text-sm font-medium text-gray-700 mb-1">
          Service Duration (minutes)
        </label>
        <input
          id="serviceDurationMinutes"
          type="number"
          {...register('serviceDurationMinutes')}
          className={`w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${
            errors.serviceDurationMinutes ? 'border-red-500' : 'border-gray-300'
          }`}
          placeholder="e.g., 30, 60, 90"
          min="1"
        />
        {errors.serviceDurationMinutes && (
          <p className="mt-1 text-sm text-red-600">{errors.serviceDurationMinutes.message}</p>
        )}
      </div>

      <div className="flex justify-end space-x-3 pt-4">
        <Button type="button" variant="secondary" onClick={onCancel} disabled={loading}>
          Cancel
        </Button>
        <Button type="submit" isLoading={loading}>
          Create Appointment
        </Button>
      </div>
    </form>
  );
};