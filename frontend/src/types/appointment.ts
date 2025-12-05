export interface AppointmentResponseDto {
  id: number;
  clientName: string;
  appointmentDate?: string;
  serviceDurationMinutes?: number;
  status?: string;
}

export interface AppointmentRequestDto {
  clientName: string;
  appointmentDate: string;
  serviceDurationMinutes?: number;
}

export interface ProblemDetails {
  status?: number;
  detail?: string;
  title?: string;
  type?: string;
}

export interface ApiError {
  status: number;
  message: string;
  details?: string;
}