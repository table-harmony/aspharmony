import { Book } from "@/lib/books";

import { Link } from "react-router-dom";

import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { AudioLinesIcon, PlusIcon } from "lucide-react";

import emptyState from "@/assets/empty.svg";
import { scrollToTop } from "@/lib/utils";
import { useUserStore } from "@/lib/userStore";

export function BookDetails({ book }: { book: Book }) {
  const user = useUserStore((state) => state.user);

  return (
    <div className="flex flex-col gap-12">
      <div className="grid gap-4">
        {book.metadata.chapters.length === 0 ? (
          <div className="flex min-h-[300px] flex-col items-center justify-center gap-6 rounded-lg border border-dashed p-8 text-center animate-in fade-in-50">
            <div className="mx-auto flex max-w-[420px] flex-col items-center justify-center gap-2 text-center">
              <h3 className="text-xl font-semibold">No chapters available</h3>
              <p className="text-muted-foreground">
                Start by adding your first chapter to this book
              </p>
            </div>
            <img
              src={emptyState}
              width={200}
              height={200}
              alt="No chapters"
              className="rounded-lg border"
            />
            {user?.id === book.author.id && (
              <Button asChild>
                <Link to={`/books/${book.id}/edit`} onClick={scrollToTop}>
                  <PlusIcon className="h-4 w-4 mr-2" />
                  Add chapters
                </Link>
              </Button>
            )}
          </div>
        ) : (
          <div className="grid gap-4">
            {book.metadata.chapters.map((chapter, index) => (
              <Card key={index} className="overflow-hidden">
                <CardContent className="p-6">
                  <div className="flex items-start gap-4">
                    <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-primary/10 text-primary">
                      {index + 1}
                    </div>
                    <div className="space-y-1">
                      <h3 className="font-semibold leading-none">
                        {chapter.title}
                      </h3>
                      <p className="text-sm text-muted-foreground line-clamp-2">
                        {chapter.content}
                      </p>
                    </div>
                  </div>
                </CardContent>
              </Card>
            ))}
          </div>
        )}
      </div>

      <Separator />

      <section className="space-y-6">
        <h2 className="text-2xl font-semibold">Audio books</h2>
        {!book.audio_books || book.audio_books.length === 0 ? (
          <div className="flex min-h-[300px] flex-col items-center justify-center gap-6 rounded-lg border border-dashed p-8 text-center animate-in fade-in-50">
            <div className="mx-auto flex max-w-[420px] flex-col items-center justify-center gap-2 text-center">
              <h3 className="text-xl font-semibold">
                No audio books available
              </h3>
              <p className="text-muted-foreground">
                Add an audio book version to enhance the reading experience
              </p>
            </div>
            <img
              src={emptyState}
              width={200}
              height={200}
              alt="No audiobooks"
            />
            {user?.id === book.author.id && (
              <Button asChild>
                <Link to={`/books/${book.id}/edit`} onClick={scrollToTop}>
                  <PlusIcon className="mr-2 h-4 w-4" />
                  Add audio book
                </Link>
              </Button>
            )}
          </div>
        ) : (
          <div className="grid gap-4">
            {book.audio_books.map((audiobook, index) => (
              <Card key={index}>
                <CardContent className="flex items-center justify-between p-6">
                  <div className="flex items-center gap-4">
                    <div className="flex h-10 w-10 items-center justify-center rounded-full bg-primary/10">
                      <AudioLinesIcon className="h-5 w-5 text-primary" />
                    </div>
                    <span className="font-medium">Audio {index + 1}</span>
                  </div>
                  <Button asChild variant="secondary">
                    <Link
                      to={audiobook.audio_url}
                      target="_blank"
                      className="flex items-center gap-2"
                    >
                      Listen
                      <AudioLinesIcon className="h-4 w-4" />
                    </Link>
                  </Button>
                </CardContent>
              </Card>
            ))}
          </div>
        )}
      </section>
    </div>
  );
}
