import { forwardRef } from 'react';
import type { TextareaHTMLAttributes } from 'react';

export interface TextAreaProps extends TextareaHTMLAttributes<HTMLTextAreaElement> {
  error?: boolean;
}

export const TextArea = forwardRef<HTMLTextAreaElement, TextAreaProps>(
  ({ error, className = '', ...props }, ref) => {
    const baseClasses =
      'w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 transition-colors resize-none';
    const errorClasses = error
      ? 'border-red-500 focus:ring-red-500'
      : 'border-gray-300 focus:ring-blue-500 focus:border-blue-500';

    return <textarea ref={ref} className={`${baseClasses} ${errorClasses} ${className}`} {...props} />;
  }
);

TextArea.displayName = 'TextArea';
