import { LoginForm } from "./form";
import { LogIn } from "lucide-react";
import { Separator } from "@/components/ui/separator";

export default function LoginPage() {
  return (
    <div className="py-8">
      <div className="container max-w-4xl mx-auto px-4 sm:px-6">
        <div className="flex flex-col gap-8">
          <section className="flex flex-col items-center text-center gap-6">
            <div className="rounded-full bg-primary/10 p-4">
              <LogIn className="h-8 w-8 text-primary" />
            </div>
            <div className="space-y-2">
              <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl">
                Welcome Back
              </h1>
              <p className="text-muted-foreground max-w-[600px] mx-auto text-balance">
                Sign in to your account to continue
              </p>
            </div>
          </section>

          <Separator />

          <section className="space-y-6 flex justify-center">
            <LoginForm />
          </section>
        </div>
      </div>
    </div>
  );
}
