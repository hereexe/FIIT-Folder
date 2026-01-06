/**
 * Decodes the JWT token from localStorage and returns the user's role.
 * @returns The user's role (e.g., "Admin", "Student", "Teacher") or null if not logged in.
 */
export function getUserRole(): string | null {
    const tokenRaw = localStorage.getItem("token");
    if (!tokenRaw) {
        return null;
    }

    try {
        const token = tokenRaw.replace(/^"|"$/g, '');
        const parts = token.split('.');
        if (parts.length !== 3) {
            console.error("Invalid token format");
            return null;
        }

        const base64Url = parts[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(
            window.atob(base64)
                .split('')
                .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        );

        const payload = JSON.parse(jsonPayload);

        // Check for role claim (standard and Microsoft claim types)
        const role =
            payload.role ||
            payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

        return role || null;
    } catch (e) {
        console.error("Failed to decode token for role", e);
        return null;
    }
}

/**
 * Returns true if the current user has the Admin role.
 */
export function isAdmin(): boolean {
    return getUserRole() === "Admin";
}
