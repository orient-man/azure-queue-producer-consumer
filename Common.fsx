#I ".paket/load/"
#load "main.group.fsx"

module Common =
    open Microsoft.WindowsAzure.Storage // Namespace for CloudStorageAccount
    open Microsoft.WindowsAzure.Storage.Queue // Namespace for Queue storage types

    // install Azure Storage Emulator: https://go.microsoft.com/fwlink/?LinkId=717179&clcid=0x409
    let connectionString = "UseDevelopmentStorage=true"

    let storageAccount = connectionString |> CloudStorageAccount.Parse
    let queueClient = storageAccount.CreateCloudQueueClient()

    let createQueue (queueClient: CloudQueueClient) name = async {
        let queue = name |> queueClient.GetQueueReference
        let! _ = queue.CreateIfNotExistsAsync() |> Async.AwaitTask
        return queue
    }

    let createQueue' = createQueue queueClient

    let queue = "testqueue" |> createQueue' |> Async.RunSynchronously