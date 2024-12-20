import { CreateLibraryForm } from "./form";
import { Library } from "lucide-react";
import { Separator } from "@/components/ui/separator";

export default function CreateLibraryPage() {
  return (
    <div className="py-8">
      <div className="container max-w-4xl mx-auto px-4 sm:px-6">
        <div className="flex flex-col gap-8">
          <section className="flex flex-col items-center text-center gap-6">
            <div className="rounded-full bg-primary/10 p-4">
              <Library className="h-8 w-8 text-primary" />
            </div>
            <div className="space-y-2">
              <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl">
                Create New Library
              </h1>
              <p className="text-muted-foreground max-w-[600px] mx-auto text-balance">
                Create a new library to share and manage books with others
              </p>
            </div>
          </section>

          <Separator />

          <section className="space-y-6 flex justify-center">
            <CreateLibraryForm />
          </section>
        </div>
      </div>
    </div>
  );
}
