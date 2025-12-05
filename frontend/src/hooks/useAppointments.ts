import { useState, useEffect, useCallback } from 'react';
import { appointmentsApi } from '../api/appointments';
import type { AppointmentResponseDto, AppointmentRequestDto, ApiError } from '../types/appointment';

export const useAppointments = () => {
  const [appointments, setAppointments] = useState<AppointmentResponseDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchAppointments = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await appointmentsApi.getAll();
      setAppointments(data);
    } catch (err) {
      const apiError = err as ApiError;
      setError(apiError.message || 'Failed to fetch appointments');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchAppointments();
  }, [fetchAppointments]);

  return {
    appointments,
    loading,
    error,
    refetch: fetchAppointments,
  };
};

export const useAppointment = (id: number) => {
  const [appointment, setAppointment] = useState<AppointmentResponseDto | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchAppointment = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await appointmentsApi.getById(id);
      setAppointment(data);
    } catch (err) {
      const apiError = err as ApiError;
      setError(apiError.message || 'Failed to fetch appointment');
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    fetchAppointment();
  }, [fetchAppointment]);

  return {
    appointment,
    loading,
    error,
    refetch: fetchAppointment,
  };
};

export const useCreateAppointment = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const createAppointment = async (data: AppointmentRequestDto) => {
    setLoading(true);
    setError(null);
    try {
      const result = await appointmentsApi.create(data);
      return result;
    } catch (err) {
      const apiError = err as ApiError;
      const errorMessage = apiError.message || 'Failed to create appointment';
      setError(errorMessage);
      throw new Error(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return {
    createAppointment,
    loading,
    error,
  };
};