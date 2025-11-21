import { z } from 'zod';
import { IssueStatus } from '../../../types/domain';

export const createIssueSchema = z.object({
  title: z
    .string()
    .min(1, 'Title is required')
    .max(200, 'Title must be less than 200 characters'),
  description: z
    .string()
    .min(1, 'Description is required')
    .max(2000, 'Description must be less than 2000 characters'),
});

export const updateIssueSchema = z.object({
  title: z
    .string()
    .min(1, 'Title is required')
    .max(200, 'Title must be less than 200 characters'),
  description: z
    .string()
    .min(1, 'Description is required')
    .max(2000, 'Description must be less than 2000 characters'),
  status: z.nativeEnum(IssueStatus),
});

export type CreateIssueFormData = z.infer<typeof createIssueSchema>;
export type UpdateIssueFormData = z.infer<typeof updateIssueSchema>;
