import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { LockKeyhole } from "lucide-react";
import { useNavigate } from "react-router-dom";

import megamind from "@/assets/megamind.jpg";

export default function ForgotPasswordPage() {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div className="w-full max-w-fit space-y-6">
        <div className="flex flex-col items-center text-center">
          <div className="rounded-full bg-primary/10 p-4">
            <LockKeyhole className="h-8 w-8 text-primary" />
          </div>
          <h1 className="mt-4 text-2xl font-bold tracking-tight sm:text-3xl">
            Forgot Your Password?
          </h1>
          <p className="mt-2 text-muted-foreground">
            Don't worry, it happens to the best of us!
          </p>
        </div>

        <Card className="flex flex-col items-center p-6 text-center">
          <img
            src={megamind}
            alt="Forgot Password"
            className="w-96 h-96 mb-6"
          />
          <Button onClick={() => navigate("/login")} className="w-full">
            Back to Login
          </Button>
        </Card>

        <p className="text-center text-sm text-muted-foreground">
          Pro tip: Have you tried "password123"?
        </p>
      </div>
    </div>
  );
}
