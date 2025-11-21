import { useForm } from 'react-hook-form';
import type { UseFormReturn } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useCreateIssue, useUpdateIssue } from './use-issues';
import type { Issue } from '../../../types/domain';
import {
  createIssueSchema,
  updateIssueSchema,
  type CreateIssueFormData,
  type UpdateIssueFormData,
} from '../types/validation';

export interface UseIssueFormParams {
  issue?: Issue;
  onSuccess: () => void;
}

export interface UseIssueFormReturn {
  form: UseFormReturn<CreateIssueFormData | UpdateIssueFormData>;
  isEditing: boolean;
  onSubmit: (data: CreateIssueFormData | UpdateIssueFormData) => Promise<void>;
  isSubmitting: boolean;
}

/**
 * Custom hook to handle issue form logic.
 * Encapsulates form state, validation, and submission logic.
 */
export function useIssueForm({ issue, onSuccess }: UseIssueFormParams): UseIssueFormReturn {
  const isEditing = !!issue;
  const createIssueMutation = useCreateIssue();
  const updateIssueMutation = useUpdateIssue();

  const form = useForm<CreateIssueFormData | UpdateIssueFormData>({
    resolver: zodResolver(isEditing ? updateIssueSchema : createIssueSchema),
    defaultValues: isEditing
      ? {
          title: issue.title,
          description: issue.description,
          status: issue.status,
        }
      : {
          title: '',
          description: '',
        },
  });

  const onSubmit = async (data: CreateIssueFormData | UpdateIssueFormData) => {
    try {
      if (isEditing) {
        await updateIssueMutation.mutateAsync({
          id: issue.id,
          data: data as UpdateIssueFormData,
        });
      } else {
        await createIssueMutation.mutateAsync(data as CreateIssueFormData);
      }
      onSuccess();
    } catch (error) {
      console.error('Failed to save issue:', error);
    }
  };

  return {
    form,
    isEditing,
    onSubmit,
    isSubmitting: form.formState.isSubmitting,
  };
}
