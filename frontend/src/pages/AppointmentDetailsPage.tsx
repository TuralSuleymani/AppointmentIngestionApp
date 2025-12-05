import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAppointment } from '../hooks/useAppointments';
import { AppointmentDetails } from '../components/appointments/AppointmentDetails';
import { Button } from '../components/common/Button';

export const AppointmentDetailsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { appointment, loading, error } = useAppointment(Number(id));

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-6">
        <Button variant="secondary" onClick={() => navigate('/')}>
          ← Back to Appointments
        </Button>
      </div>

      <AppointmentDetails
        appointment={appointment}
        loading={loading}
        error={error}
      />
    </div>
  );
};