// For backward compatibility, we'll export a component that uses the hook
import { useIssueForm } from '../../hooks/use-issue-form';
import { IssueFormView } from './IssueFormView';
import type { Issue } from '../../../../types/domain';

export interface IssueFormProps {
  issue?: Issue;
  onSuccess: () => void;
  onCancel: () => void;
}

export function IssueForm({ issue, onSuccess, onCancel }: IssueFormProps) {
  const { form, isEditing, onSubmit, isSubmitting } = useIssueForm({ issue, onSuccess });

  return (
    <IssueFormView
      form={form}
      isEditing={isEditing}
      isSubmitting={isSubmitting}
      onSubmit={onSubmit}
      onCancel={onCancel}
    />
  );
}

// Also export the view for testing
export { IssueFormView } from './IssueFormView';
export type { IssueFormViewProps } from './IssueFormView';
