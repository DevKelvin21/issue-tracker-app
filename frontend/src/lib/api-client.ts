import axios from 'axios';
import type { ProblemDetails } from '../types/common';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5094/api';

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Response interceptor for error handling
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.data) {
      // Backend returns RFC 9457 Problem Details
      const problemDetails: ProblemDetails = error.response.data;
      error.message = problemDetails.title || error.message;
      error.problemDetails = problemDetails;
    }
    return Promise.reject(error);
  }
);

export default apiClient;
