import type { IssueStatus } from '../../../types/domain';

/**
 * DTO for creating a new issue
 */
export interface CreateIssueDto {
  title: string;
  description: string;
}

/**
 * DTO for updating an existing issue
 */
export interface UpdateIssueDto {
  title: string;
  description: string;
  status: IssueStatus;
}
