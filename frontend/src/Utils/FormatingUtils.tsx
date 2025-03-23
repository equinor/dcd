export function truncateText(text: string, maxLength: number): string {
    return (text.length + 3) > maxLength ? `${text.slice(0, maxLength)}...` : text
}
