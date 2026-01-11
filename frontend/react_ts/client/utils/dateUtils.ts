/**
 * Generates an array of years from startYear to the current year (inclusive).
 * The array is sorted in descending order (newest first).
 * @param startYear The first year to include in the range.
 * @returns An array of years as strings.
 */
export function getYearRange(startYear: number): string[] {
    const currentYear = new Date().getFullYear();
    const years: string[] = [];
    for (let year = currentYear; year >= startYear; year--) {
        years.push(year.toString());
    }
    return years;
}
