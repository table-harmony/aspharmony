import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";

function BookCardSkeleton() {
  return (
    <Card className="overflow-hidden">
      <Skeleton className="aspect-[3/2] w-full" />
      <CardContent className="flex flex-col gap-2.5 p-4">
        <div className="space-y-1">
          <Skeleton className="h-5 w-2/3" />
          <Skeleton className="h-4 w-1/3" />
        </div>
        <Skeleton className="h-4 w-full" />
        <Skeleton className="h-4 w-full" />
      </CardContent>
      <CardFooter className="p-4 pt-0">
        <div className="flex w-full items-center justify-between gap-2">
          <Skeleton className="h-9 w-24" />
          <Skeleton className="h-4 w-20" />
        </div>
      </CardFooter>
    </Card>
  );
}

export function BooksListSkeleton() {
  return (
    <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
      {Array.from({ length: 8 }).map((_, i) => (
        <BookCardSkeleton key={i} />
      ))}
    </div>
  );
}
