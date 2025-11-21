import type { ProblemDetails } from './api';

/**
 * Custom error class for API errors.
 * Extends the standard Error class with additional API-specific information.
 */
export class ApiError extends Error {
  constructor(
    message: string,
    public readonly statusCode: number,
    public readonly problemDetails?: ProblemDetails
  ) {
    super(message);
    this.name = 'ApiError';

    // Maintains proper stack trace for where our error was thrown (only available on V8)
    if (typeof (Error as any).captureStackTrace === 'function') {
      (Error as any).captureStackTrace(this, ApiError);
    }
  }

  /**
   * Check if this is a validation error (400 Bad Request)
   */
  isValidationError(): boolean {
    return this.statusCode === 400;
  }

  /**
   * Check if this is a not found error (404 Not Found)
   */
  isNotFoundError(): boolean {
    return this.statusCode === 404;
  }

  /**
   * Check if this is an unauthorized error (401 Unauthorized)
   */
  isUnauthorizedError(): boolean {
    return this.statusCode === 401;
  }

  /**
   * Check if this is a forbidden error (403 Forbidden)
   */
  isForbiddenError(): boolean {
    return this.statusCode === 403;
  }

  /**
   * Check if this is a server error (5xx)
   */
  isServerError(): boolean {
    return this.statusCode >= 500 && this.statusCode < 600;
  }

  /**
   * Get validation errors if available
   */
  getValidationErrors(): Record<string, string[]> | undefined {
    return this.problemDetails?.errors;
  }

  /**
   * Get a user-friendly error message
   */
  getUserMessage(): string {
    if (this.problemDetails?.detail) {
      return this.problemDetails.detail;
    }

    if (this.isValidationError()) {
      return 'Please check your input and try again.';
    }

    if (this.isNotFoundError()) {
      return 'The requested resource was not found.';
    }

    if (this.isUnauthorizedError()) {
      return 'You are not authorized to perform this action.';
    }

    if (this.isServerError()) {
      return 'An error occurred on the server. Please try again later.';
    }

    return this.message || 'An unexpected error occurred.';
  }
}
