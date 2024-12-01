import { Button, ButtonProps } from "@/components/ui/button";

import { logout } from "@/lib/auth";
import { useUserStore } from "@/lib/userStore";
import { Loader2, LogOutIcon } from "lucide-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "sonner";

export function LogoutButton({ ...props }: ButtonProps) {
  const navigate = useNavigate();
  const setUser = useUserStore((state) => state.setUser);
  const [isLoading, setIsLoading] = useState(false);

  const handleLogout = async () => {
    try {
      setIsLoading(true);
      setUser(null);
      await logout();

      toast.success("Logged out successfully");

      navigate("/");
    } catch {
      toast.error("Failed to logout");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Button onClick={handleLogout} disabled={isLoading} {...props}>
      {isLoading ? (
        <Loader2 className="h-4 w-4 animate-spin" />
      ) : (
        <LogOutIcon className="h-4 w-4" />
      )}
      {isLoading ? "Logging out..." : "Logout"}
    </Button>
  );
}
