// formats date display (eg "Friday, April 3")
export function formatDateLabel(value: string) {
  const date = new Date(value);

  return new Intl.DateTimeFormat('en-US', {
    weekday: 'long',
    month: 'long',
    day: 'numeric',
  }).format(date);
}