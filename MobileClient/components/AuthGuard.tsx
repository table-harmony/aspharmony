import React, { useEffect } from "react";
import { useUserStore } from "../services/userStore";
import { useNavigate } from "react-router-native";
import { authService } from "../services/auth";

interface AuthGuardProps {
  children: React.ReactNode;
  requireAuth?: boolean;
}

export const AuthGuard: React.FC<AuthGuardProps> = ({
  children,
  requireAuth = true,
}) => {
  const { isAuthenticated } = useUserStore();
  const navigate = useNavigate();

  useEffect(() => {
    const checkAuth = async () => {
      const isAuth = await authService.checkAuth();
      if (requireAuth && !isAuth) {
        navigate("/login");
      } else if (!requireAuth && isAuth) {
        navigate("/home");
      }
    };

    checkAuth();
  }, [isAuthenticated, requireAuth, navigate]);

  return <>{children}</>;
};
