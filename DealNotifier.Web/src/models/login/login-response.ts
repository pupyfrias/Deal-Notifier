export type LoginResponse = {
    id: string;
    userName: string;
    email: string;
    roles: string[];
    isVerified: boolean;
    accessToken: string;
    expiresIn: number;
  }