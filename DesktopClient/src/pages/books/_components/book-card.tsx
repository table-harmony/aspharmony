import { Book } from "@/lib/books";

import { Link } from "react-router-dom";

import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { BookIcon, BookOpenIcon } from "lucide-react";
import { scrollToTop } from "@/lib/utils";

export function BookCard({ book }: { book: Book }) {
  return (
    <Card className="group relative flex flex-col overflow-hidden">
      <div className="relative aspect-[3/2] overflow-hidden">
        <img
          src={book.metadata.image_url}
          alt={book.metadata.title}
          className="object-cover transition-transform duration-300 group-hover:scale-105"
        />
      </div>
      <CardContent className="flex flex-col flex-1 gap-2.5 p-4">
        <div className="space-y-1">
          <h3 className="font-semibold text-lg leading-none tracking-tight">
            {book.metadata.title}
          </h3>
          <p className="text-sm text-muted-foreground">
            By {book.author.username}
          </p>
        </div>
        <p className="text-sm text-muted-foreground line-clamp-2">
          {book.metadata.description}
        </p>
      </CardContent>
      <CardFooter className="p-4 pt-0">
        <div className="flex w-full items-center justify-between gap-2">
          <Button variant="outline" size="sm" asChild>
            <Link to={`/books/${book.id}`} onClick={scrollToTop}>
              <BookIcon className="h-4 w-4" />
              View
            </Link>
          </Button>
          <div className="flex items-center gap-1 text-sm text-muted-foreground">
            <BookOpenIcon className="h-4 w-4" />
            <span>{book.metadata.chapters.length} chapters</span>
          </div>
        </div>
      </CardFooter>
    </Card>
  );
}
