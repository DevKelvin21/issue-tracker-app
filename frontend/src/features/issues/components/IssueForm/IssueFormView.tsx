import type { UseFormReturn } from 'react-hook-form';
import { Card, Button, Input, TextArea, Select, FormField } from '../../../../components/ui';
import type { IssueStatus } from '../../../../types/domain';
import { getStatusLabel } from '../../../../lib/utils';
import type { CreateIssueFormData, UpdateIssueFormData } from '../../types/validation';

export interface IssueFormViewProps {
  form: UseFormReturn<CreateIssueFormData | UpdateIssueFormData>;
  isEditing: boolean;
  isSubmitting: boolean;
  onSubmit: (data: CreateIssueFormData | UpdateIssueFormData) => Promise<void>;
  onCancel: () => void;
}

export function IssueFormView({ form, isEditing, isSubmitting, onSubmit, onCancel }: IssueFormViewProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = form;

  return (
    <Card>
      <h2 className="text-2xl font-bold text-gray-900 mb-6">{isEditing ? 'Edit Issue' : 'Create New Issue'}</h2>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <FormField label="Title" htmlFor="title" error={errors.title?.message} required>
          <Input
            id="title"
            type="text"
            placeholder="Enter issue title"
            error={!!errors.title}
            {...register('title')}
          />
        </FormField>

        <FormField label="Description" htmlFor="description" error={errors.description?.message} required>
          <TextArea
            id="description"
            rows={5}
            placeholder="Enter issue description"
            error={!!errors.description}
            {...register('description')}
          />
        </FormField>

        {isEditing && (
          <FormField label="Status" htmlFor="status" error={errors.root?.status?.message} required>
            <Select id="status" error={!!errors.root?.status} {...register('status', { valueAsNumber: true })}>
              {[1, 2, 3].map((status) => (
                <option key={status} value={status}>
                  {getStatusLabel(status as IssueStatus)}
                </option>
              ))}
            </Select>
          </FormField>
        )}

        <div className="flex gap-3 pt-2">
          <Button type="submit" disabled={isSubmitting} isLoading={isSubmitting}>
            {isEditing ? 'Update Issue' : 'Create Issue'}
          </Button>
          <Button type="button" variant="secondary" onClick={onCancel}>
            Cancel
          </Button>
        </div>
      </form>
    </Card>
  );
}
