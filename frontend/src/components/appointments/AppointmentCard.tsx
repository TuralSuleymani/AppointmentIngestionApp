import React from 'react';
import type { AppointmentResponseDto } from '../../types/appointment';

interface AppointmentCardProps {
  appointment: AppointmentResponseDto;
  onViewDetails: (id: number) => void;
}

export const AppointmentCard: React.FC<AppointmentCardProps> = ({
  appointment,
  onViewDetails,
}) => {
  return (
    <div className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
      <div className="flex justify-between items-start mb-4">
        <h3 className="text-lg font-semibold text-gray-900">{appointment.clientName}</h3>
        {appointment.status && (
          <span className="px-2 py-1 text-xs font-medium rounded-full bg-blue-100 text-blue-800">
            {appointment.status}
          </span>
        )}
      </div>
      
      {appointment.appointmentDate && (
        <p className="text-sm text-gray-600 mb-2">
          <span className="font-medium">Date:</span>{' '}
          {new Date(appointment.appointmentDate).toLocaleString()}
        </p>
      )}
      
      {appointment.serviceDurationMinutes && (
        <p className="text-sm text-gray-600 mb-4">
          <span className="font-medium">Duration:</span>{' '}
          {appointment.serviceDurationMinutes} minutes
        </p>
      )}
      
      <button
        onClick={() => onViewDetails(appointment.id)}
        className="text-blue-600 hover:text-blue-800 text-sm font-medium"
      >
        View Details →
      </button>
    </div>
  );
};