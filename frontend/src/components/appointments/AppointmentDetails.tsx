import React from 'react';
import type { AppointmentResponseDto } from '../../types/appointment';
import { LoadingSpinner } from '../common/LoadingSpinner';

interface AppointmentDetailsProps {
  appointment: AppointmentResponseDto | null;
  loading: boolean;
  error: string | null;
}

export const AppointmentDetails: React.FC<AppointmentDetailsProps> = ({
  appointment,
  loading,
  error,
}) => {
  if (loading) return <LoadingSpinner />;
  if (error) return <div className="text-red-600 p-4">{error}</div>;
  if (!appointment) return <div className="text-gray-600 p-4">Appointment not found</div>;

  return (
    <div className="bg-white rounded-lg shadow-md p-6">
      <h2 className="text-2xl font-bold text-gray-900 mb-4">{appointment.clientName}</h2>
      
      <div className="space-y-3">
        <div>
          <span className="text-sm font-medium text-gray-500">ID:</span>
          <p className="text-gray-900">{appointment.id}</p>
        </div>

        {appointment.appointmentDate && (
          <div>
            <span className="text-sm font-medium text-gray-500">Appointment Date:</span>
            <p className="text-gray-900">
              {new Date(appointment.appointmentDate).toLocaleString()}
            </p>
          </div>
        )}

        {appointment.serviceDurationMinutes && (
          <div>
            <span className="text-sm font-medium text-gray-500">Service Duration:</span>
            <p className="text-gray-900">{appointment.serviceDurationMinutes} minutes</p>
          </div>
        )}

        {appointment.status && (
          <div>
            <span className="text-sm font-medium text-gray-500">Status:</span>
            <p className="text-gray-900">{appointment.status}</p>
          </div>
        )}
      </div>
    </div>
  );
};