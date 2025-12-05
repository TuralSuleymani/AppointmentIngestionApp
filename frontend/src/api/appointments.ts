import { apiClient } from './client';
import type { AppointmentResponseDto, AppointmentRequestDto } from '../types/appointment';

interface IngestResponse {
  id: number;
  message: string;
}

export const appointmentsApi = {
  getAll: async (): Promise<AppointmentResponseDto[]> => {
    console.log(' Fetching all appointments...');
    return apiClient.get<AppointmentResponseDto[]>('/appointments');
  },

  getById: async (id: number): Promise<AppointmentResponseDto> => {
    console.log(` Fetching appointment with ID: ${id}`);
    return apiClient.get<AppointmentResponseDto>(`/appointments/${id}`);
  },

  create: async (data: AppointmentRequestDto): Promise<IngestResponse> => {
    console.log(' Creating new appointment with data:', data);
    return apiClient.post<IngestResponse, AppointmentRequestDto>('/appointments/ingest', data);
  },
};