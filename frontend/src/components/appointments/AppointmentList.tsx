import React from 'react';
import { AppointmentCard } from './AppointmentCard';
import { LoadingSpinner } from '../common/LoadingSpinner';
import type { AppointmentResponseDto } from '../../types/appointment';

interface AppointmentListProps {
  appointments: AppointmentResponseDto[];
  loading: boolean;
  error: string | null;
  onViewDetails: (id: number) => void;
}

export const AppointmentList: React.FC<AppointmentListProps> = ({
  appointments,
  loading,
  error,
  onViewDetails,
}) => {
  if (loading) return <LoadingSpinner />;
  if (error) {
    return (
      <div className="bg-red-50 border border-red-200 rounded-md p-4">
        <p className="text-red-800">{error}</p>
      </div>
    );
  }
  if (appointments.length === 0) {
    return (
      <div className="text-center py-12">
        <p className="text-gray-500 text-lg">No appointments found</p>
        <p className="text-gray-400 text-sm mt-2">Create your first appointment to get started</p>
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {appointments.map((appointment) => (
        <AppointmentCard
          key={appointment.id}
          appointment={appointment}
          onViewDetails={onViewDetails}
        />
      ))}
    </div>
  );
};