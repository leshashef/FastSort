using System.Diagnostics;

Random random = new Random();
int[] a = new int[100_000_000_0];
for (int i = 0; i < a.Length; i++)
{
    a[i] = random.Next(int.MinValue, int.MaxValue);
}

int l = 0;
int r = a.Length - 1;

Stopwatch stopwatch = Stopwatch.StartNew();
SortBase.MergeSort(a, threadCount: 8); // быстро
//Array.Sort(a); // слишком быстро

//a = a.OrderBy(a => a).ToArray(); // быстро
//BubbleSort(a); // слишком долго...
stopwatch.Stop();
Console.WriteLine($"Время сортировки: {stopwatch.ElapsedMilliseconds} ms \n");

for (int i = 0; i < 500; i++)
{
    Console.WriteLine(a[i]);
}



public class SortBase
{
    public static Task MergeSort(int[] a, int l, int r, int threadCount = 2)
    {
        int q;

        if (l < r)
        {
            q = (l + r) / 2;

            //  await Task.WhenAll(MergeSort(a, l, q), MergeSort(a, q + 1, r));

            if (threadCount > 1)
            {
                threadCount -= 2;
                
                //Task waitTask = Task.WhenAll(MergeSort(a, l, q, threadCount), MergeSort(a, q + 1, r, threadCount));
                //waitTask.Wait();
                Thread thread = new Thread(() => { MergeSort(a, l, q, threadCount); });
                Thread thread2 = new Thread(() => { MergeSort(a, q + 1, r, threadCount); });

                thread.Start();
                thread2.Start();
                thread.Join();
                thread2.Join();
            }
            else
            {
                MergeSort(a, l, q, threadCount);
                MergeSort(a, q + 1, r, threadCount);
            }
            // MergeSort(a, q + 1, r);


            Merge(a, l, q, r);


        }

        return Task.CompletedTask;
    }
    public static void MergeSort(int[] a, int threadCount = 2)
    {
        int l = 0;
        int r = a.Length - 1;

        MergeSort(a, l, r, threadCount);
    }

    static void Merge(int[] a, int l, int m, int r)
    {
        int i, j, k;

        int n1 = m - l + 1;
        int n2 = r - m;

        int[] left = new int[n1 + 1];
        int[] right = new int[n2 + 1];

        for (i = 0; i < n1; i++)
        {
            left[i] = a[l + i];
        }

        for (j = 1; j <= n2; j++)
        {
            right[j - 1] = a[m + j];
        }

        left[n1] = int.MaxValue;
        right[n2] = int.MaxValue;

        i = 0;
        j = 0;

        for (k = l; k <= r; k++)
        {
            if (left[i] < right[j])
            {
                a[k] = left[i];
                i = i + 1;
            }
            else
            {
                a[k] = right[j];
                j = j + 1;
            }
        }
    }


}



//static void BubbleSort(int[] array)
//{
//    bool swapped;
//    for (int i = 0; i < array.Length - 1; i++)
//    {
//        swapped = false;
//        for (int j = 0; j < array.Length - i - 1; j++)
//        {
//            if (array[j] > array[j + 1])
//            {
//                // Swap elements
//                int temp = array[j];
//                array[j] = array[j + 1];
//                array[j + 1] = temp;
//                swapped = true;
//            }
//        }
//        // Если на каком-то проходе не было обменов, массив отсортирован
//        if (!swapped)
//            break;
//    }
//}