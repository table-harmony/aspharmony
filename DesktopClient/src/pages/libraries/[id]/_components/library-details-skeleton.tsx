import { Card, CardContent } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Separator } from "@/components/ui/separator";

export function LibraryDetailsSkeleton() {
  return (
    <div className="flex flex-col gap-8">
      <div className="flex flex-col gap-6">
        <div className="space-y-4">
          <Skeleton className="h-9 w-1/3" />
          <Skeleton className="h-6 w-1/4" />
          <div className="flex gap-4 pt-2">
            <Skeleton className="h-10 w-32" />
            <Skeleton className="h-10 w-32" />
          </div>
        </div>
      </div>

      <Separator />

      <div className="space-y-6">
        <Skeleton className="h-8 w-32" />
        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
          {Array.from({ length: 3 }).map((_, index) => (
            <Card key={index}>
              <CardContent className="p-6">
                <div className="flex flex-col gap-4">
                  <Skeleton className="h-40" />
                  <div className="space-y-2">
                    <Skeleton className="h-5 w-2/3" />
                    <Skeleton className="h-4 w-full" />
                  </div>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>
    </div>
  );
}
