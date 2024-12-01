import { BooksList } from "./_components/books-list";
import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";
import { PlusIcon, BookOpenText } from "lucide-react";
import { Separator } from "@/components/ui/separator";
import { scrollToTop } from "@/lib/utils";

export default function BooksPage() {
  return (
    <div className="py-8">
      <div className="container max-w-6xl mx-auto px-4 sm:px-6">
        <div className="flex flex-col gap-8">
          <section className="flex flex-col items-center text-center gap-6">
            <div className="rounded-full bg-primary/10 p-4">
              <BookOpenText className="h-8 w-8 text-primary" />
            </div>
            <div className="space-y-2">
              <h1 className="text-3xl font-bold tracking-tighter sm:text-4xl">
                Book Collection
              </h1>
              <p className="text-muted-foreground max-w-[600px] mx-auto text-balance">
                Explore your personal library, create new books, and manage your
                collection all in one place.
              </p>
            </div>
            <Button asChild size="lg">
              <Link to="/books/create" onClick={scrollToTop}>
                <PlusIcon className="mr-2 h-4 w-4" />
                Create New Book
              </Link>
            </Button>
          </section>

          <Separator />

          <section className="space-y-6">
            <BooksList />
          </section>
        </div>
      </div>
    </div>
  );
}
