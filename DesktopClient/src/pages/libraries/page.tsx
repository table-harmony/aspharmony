import { LibrariesList } from "./_components/libraries-list";
import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";
import { PlusIcon, Library } from "lucide-react";
import { Separator } from "@/components/ui/separator";
import { scrollToTop } from "@/lib/utils";

export default function LibrariesPage() {
  return (
    <div className="min-h-screen py-8">
      <div className="container max-w-6xl mx-auto px-4 sm:px-6">
        <div className="flex flex-col gap-8 animate-in fade-in-50">
          <section className="flex flex-col items-center text-center gap-6">
            <div className="rounded-full bg-primary/10 p-4">
              <Library className="h-8 w-8 text-primary" />
            </div>
            <div className="space-y-2">
              <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl">
                Libraries
              </h1>
              <p className="text-muted-foreground max-w-[600px] mx-auto text-balance">
                Join existing libraries or create your own to share and manage
                books with others.
              </p>
            </div>
            <Button asChild size="lg">
              <Link to="/libraries/create" onClick={scrollToTop}>
                <PlusIcon className="h-4 w-4" />
                Create Library
              </Link>
            </Button>
          </section>

          <Separator />

          <section className="space-y-6">
            <LibrariesList />
          </section>
        </div>
      </div>
    </div>
  );
}
