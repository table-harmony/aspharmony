import { LoginForm } from "./form";
import { LogIn } from "lucide-react";

export default function LoginPage() {
  return (
    <div className="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div className="flex flex-col items-center text-center">
          <div className="rounded-full bg-primary/10 p-5 mb-6">
            <LogIn className="h-10 w-10 text-primary" />
          </div>
          <h1 className="text-3xl font-extrabold tracking-tight sm:text-4xl mb-2">
            Welcome Back
          </h1>
          <p className="text-muted-foreground max-w-sm mx-auto text-balance">
            Sign in to your account to continue
          </p>
        </div>

        <div className="mt-8 bg-card rounded-lg shadow-sm border">
          <LoginForm />
        </div>
      </div>
    </div>
  );
}
