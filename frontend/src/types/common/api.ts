/**
 * RFC 9457 Problem Details for HTTP APIs
 * Standard error response structure
 */
export interface ProblemDetails {
  type?: string;
  title: string;
  status: number;
  detail?: string;
  instance?: string;
  errors?: Record<string, string[]>;
}
