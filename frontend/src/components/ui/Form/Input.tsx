import { forwardRef } from 'react';
import type { InputHTMLAttributes } from 'react';

export interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  error?: boolean;
}

export const Input = forwardRef<HTMLInputElement, InputProps>(({ error, className = '', ...props }, ref) => {
  const baseClasses =
    'w-full px-3 py-2 border rounded-md focus:outline-none focus:ring-2 transition-colors';
  const errorClasses = error
    ? 'border-red-500 focus:ring-red-500'
    : 'border-gray-300 focus:ring-blue-500 focus:border-blue-500';

  return <input ref={ref} className={`${baseClasses} ${errorClasses} ${className}`} {...props} />;
});

Input.displayName = 'Input';
