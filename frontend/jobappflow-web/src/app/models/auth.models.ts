export interface AuthUser {
  id: string;
  userName: string;
  email: string | null;
}

export interface AuthSession {
  accessToken: string;
  user: AuthUser;
}

export interface LoginRequest {
  emailOrUsername: string;
  password: string;
}
