#load "Common.fsx"

open Microsoft.WindowsAzure.Storage // Namespace for CloudStorageAccount
open Microsoft.WindowsAzure.Storage.Queue // Namespace for Queue storage types
open Common

let postMsg (queue: CloudQueue) (msg: string) = msg |> CloudQueueMessage |> queue.AddMessageAsync |> Async.AwaitTask

let postMsg' = postMsg Common.queue

"Hello!" |> postMsg' |> Async.RunSynchronously