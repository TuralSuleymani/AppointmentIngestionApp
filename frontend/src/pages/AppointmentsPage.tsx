import React, { useState } from 'react';
import { useAppointments } from '../hooks/useAppointments';
import { AppointmentList } from '../components/appointments/AppointmentList';
import { AppointmentForm } from '../components/appointments/AppointmentForm';
import { Modal } from '../components/common/Modal';
import { Button } from '../components/common/Button';
import { useNavigate } from 'react-router-dom';

export const AppointmentsPage: React.FC = () => {
  const { appointments, loading, error, refetch } = useAppointments();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const navigate = useNavigate();

  const handleCreateSuccess = () => {
    setIsModalOpen(false);
    refetch();
  };

  const handleViewDetails = (id: number) => {
    navigate(`/appointments/${id}`);
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Appointments</h1>
        <Button onClick={() => setIsModalOpen(true)}>
          Create Appointment
        </Button>
      </div>

      <AppointmentList
        appointments={appointments}
        loading={loading}
        error={error}
        onViewDetails={handleViewDetails}
      />

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title="Create New Appointment"
      >
        <AppointmentForm
          onSuccess={handleCreateSuccess}
          onCancel={() => setIsModalOpen(false)}
        />
      </Modal>
    </div>
  );
};