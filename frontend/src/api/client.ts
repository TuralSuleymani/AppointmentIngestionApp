import type { ProblemDetails, ApiError } from '../types/appointment';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7104/api';

class ApiClient {
  private baseUrl: string;

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl;
    console.log(' API Client initialized with base URL:', baseUrl);
  }

  private async handleResponse<T>(response: Response): Promise<T> {
    console.log(' Response status:', response.status, response.statusText);
    
    if (!response.ok) {
      let errorMessage = `HTTP error! status: ${response.status}`;
      let details: string | undefined;

      try {
        const problemDetails: ProblemDetails = await response.json();
        console.log(' Error response body:', problemDetails);
        errorMessage = problemDetails.detail || problemDetails.title || errorMessage;
        details = problemDetails.detail;
      } catch {
        console.log(' Could not parse error response');
      }

      const error: ApiError = {
        status: response.status,
        message: errorMessage,
        details,
      };
      throw error;
    }

    const data = await response.json();
    console.log(' Response data:', data);
    return data;
  }

  async get<T>(endpoint: string): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;
    console.log(' GET Request:', url);
    
    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    return this.handleResponse<T>(response);
  }

  async post<T, D>(endpoint: string, data: D): Promise<T> {
    const url = `${this.baseUrl}${endpoint}`;
    console.log(' POST Request:', url);
    console.log('Request body:', JSON.stringify(data));
    
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    return this.handleResponse<T>(response);
  }

  async delete(endpoint: string): Promise<void> {
    const url = `${this.baseUrl}${endpoint}`;
    console.log(' DELETE Request:', url);
    
    const response = await fetch(url, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
  }
}

export const apiClient = new ApiClient(API_BASE_URL);