import { forwardRef } from 'react';
import type { SelectHTMLAttributes } from 'react';

export interface SelectProps extends SelectHTMLAttributes<HTMLSelectElement> {
  error?: boolean;
}

export const Select = forwardRef<HTMLSelectElement, SelectProps>(({ error, className = '', children, ...props }, ref) => {
  const baseClasses =
    'w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 transition-colors bg-white';
  const errorClasses = error
    ? 'border-red-500 focus:ring-red-500'
    : 'border-gray-300 focus:ring-blue-500 focus:border-blue-500';

  return (
    <select ref={ref} className={`${baseClasses} ${errorClasses} ${className}`} {...props}>
      {children}
    </select>
  );
});

Select.displayName = 'Select';
