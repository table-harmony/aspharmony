import { RegisterForm } from "./form";
import { UserPlus } from "lucide-react";

export default function RegisterPage() {
  return (
    <div className="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div className="flex flex-col items-center text-center">
          <div className="rounded-full bg-primary/10 p-5 mb-6">
            <UserPlus className="h-10 w-10 text-primary" />
          </div>
          <h1 className="text-3xl font-extrabold tracking-tight sm:text-4xl mb-2">
            Create an Account
          </h1>
          <p className="text-muted-foreground max-w-sm mx-auto text-balance">
            Sign up to start creating and sharing books
          </p>
        </div>

        <div className="mt-8 bg-card rounded-lg shadow-sm border">
          <RegisterForm />
        </div>
      </div>
    </div>
  );
}
