import { z } from 'zod';

export const appointmentSchema = z.object({
  clientName: z.string()
    .min(1, 'Client name is required')
    .max(100, 'Client name must be less than 100 characters'),
  appointmentDate: z.string()
    .min(1, 'Appointment date is required'),
  serviceDurationMinutes: z.coerce.number()
    .positive('Service duration must be a positive number')
    .int('Service duration must be a whole number')
    .optional()
    .or(z.literal('')),
});

export type AppointmentFormData = z.infer<typeof appointmentSchema>;