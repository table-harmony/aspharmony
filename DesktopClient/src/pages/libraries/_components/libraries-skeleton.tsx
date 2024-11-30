import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";

function LibraryCardSkeleton() {
  return (
    <Card className="overflow-hidden">
      <CardContent className="flex flex-col gap-2.5 p-4">
        <div className="space-y-1">
          <Skeleton className="h-5 w-2/3" />
          <Skeleton className="h-4 w-1/3" />
        </div>
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

export function LibrariesListSkeleton() {
  return (
    <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
      {Array.from({ length: 6 }).map((_, i) => (
        <LibraryCardSkeleton key={i} />
      ))}
    </div>
  );
}
