export interface User {
    name: string,
    username: string,
    token: string;
}

export interface UserFormValues {
    email: string;
    password: string;
    name?: string;
    username?: string;
}
