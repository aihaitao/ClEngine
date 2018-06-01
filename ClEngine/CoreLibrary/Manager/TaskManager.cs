using System;
using System.Collections.Generic;
using System.Threading;
using ClEngine.Core.ProjectCreator;
using ClEngine.CoreLibrary.Task;
using ClEngine.Properties;

namespace ClEngine.CoreLibrary.Manager
{
    public class TaskManager : Singleton<TaskManager>
    {
        #region Fields
        int _asyncTasks;

        List<GlueTask> mSyncedActions = new List<GlueTask>();
        static object mSyncLockObject = new object();


        List<GlueTask> mActiveAsyncTasks = new List<GlueTask>();

        #endregion

        public event Action TaskAddedOrRemoved;

        #region Properties

        public int SyncTaskTasks => mSyncedActions.Count;

        public bool AreAllAsyncTasksDone
        {
            get
            {
                lock (mActiveAsyncTasks)
                {
                    return mActiveAsyncTasks.Count == 0;
                }
            }
        }

        public int TaskCount
        {
            get
            {
                lock (mActiveAsyncTasks)
                {
                    return mActiveAsyncTasks.Count + _asyncTasks + mSyncedActions.Count;
                }
            }
        }

        public string CurrentTask
        {
            get
            {
                string toReturn = "";

                if (IsTaskProcessingEnabled == false)
                {
                    toReturn += $"{Resources.DisabledTask}:\n";
                }

                lock (mActiveAsyncTasks)
                {
                    if (mActiveAsyncTasks.Count != 0)
                    {
                        // This could update while we're looping. We don't want to throw errors, don't want to lock anything, 
                        // so just handle it with a try catch:
                        try
                        {
                            lock (mActiveAsyncTasks)
                            {
                                foreach (var item in mActiveAsyncTasks)
                                {
                                    toReturn += item.DisplayInfo + "\n";
                                }
                            }
                        }
                        catch
                        {
                            // do nothing
                        }

                    }
                }

                lock (mSyncLockObject)
                {
                    if (mSyncedActions.Count != 0)
                    {
                        try
                        {
                            toReturn += mSyncedActions[0].DisplayInfo;
                        }
                        catch
                        {
                            // do nothing
                        }
                    }
                }


                return toReturn;
            }
        }

        bool _isTaskProcessingEnabled;
        /// <summary>
        /// Whether to process tasks - if this is false, then tasks will not be processed.
        /// </summary>
        public bool IsTaskProcessingEnabled
        {
            get => _isTaskProcessingEnabled;
            set
            {
                var turnedOn = value && _isTaskProcessingEnabled == false;
                _isTaskProcessingEnabled = value;
                if (turnedOn)
                {
                    ProcessNextSync();

                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a task which can execute simultaneously with other tasks
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="details">The details of the task, to be displayed in the tasks window.</param>
        public void AddAsyncTask(Action action, string details)
        {

            ThreadPool.QueueUserWorkItem(
                (arg) => ExecuteActionSync(action, details));
        }

        void CallTaskAddedOrRemoved()
        {
            if (TaskAddedOrRemoved != null)
            {
                TaskAddedOrRemoved();
            }
        }

        void ExecuteActionSync(Action action, string details)
        {
            var glueTask = new GlueTask
            {
                Action = action,
                DisplayInfo = details
            };

            lock (mActiveAsyncTasks)
            {
                mActiveAsyncTasks.Add(glueTask);
            }

            CallTaskAddedOrRemoved();
            action();

            lock (mActiveAsyncTasks)
            {
                mActiveAsyncTasks.Remove(glueTask);
            }
            _asyncTasks--;

            // not sure why but this can go into the negative...
            _asyncTasks = Math.Max(_asyncTasks, 0);

            CallTaskAddedOrRemoved();



        }


        public void WaitForAllTasksFinished(bool pumpEvents)
        {
            while (!AreAllAsyncTasksDone)
            {
                Thread.Sleep(50);
#if GLUE
                if (pumpEvents)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
#endif
            }
        }



        /// <summary>
        /// Adds an action to be executed, guaranteeing that no other actions will be executed at the same time as this.
        /// Actions added will be executed in the order they were added (fifo).
        /// </summary>
        public void AddSync(Action action, string displayInfo)
        {
            AddSync(action, displayInfo, isHighPriority: false);
        }

        /// <summary>
        /// Adds an action to be executed, guaranteeing that no other actions will be executed at the same time as this.
        /// Actions added will be executed in the order they were added (fifo).
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="displayInfo">The details of the task, to de bisplayed in the tasks window.</param>
        /// <param name="isHighPriority">Whether to attempt to run the action immediately - useful for UI tasks</param>
        public void AddSync(Action action, string displayInfo, bool isHighPriority)
        {
            var glueTask = new GlueTask
            {
                Action = action,
                DisplayInfo = displayInfo
            };

            bool shouldProcess;

            lock (mSyncLockObject)
            {
                if (isHighPriority)
                {
                    if (mSyncedActions.Count > 0)
                    {
                        // don't insert at 0, finish the current task, but insert at 1:
                        mSyncedActions.Insert(1, glueTask);
                    }
                    else
                    {
                        mSyncedActions.Add(glueTask);
                    }
                }
                else
                {
                    mSyncedActions.Add(glueTask);
                }
                shouldProcess = mSyncedActions.Count == 1 && IsTaskProcessingEnabled;
            }
            CallTaskAddedOrRemoved();
            if (shouldProcess)
            {
                ProcessNextSync();
            }
        }

        private void ProcessNextSync()
        {
            Action toProcess = null;
            lock (mSyncLockObject)
            {
                if (mSyncedActions.Count > 0)
                {
                    toProcess = mSyncedActions[0].Action;
                }
            }

            if (toProcess != null)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    toProcess();

                    bool shouldProcess;

                    lock (mSyncLockObject)
                    {

                        if (mSyncedActions.Count > 0)
                        {
                            mSyncedActions.RemoveAt(0);
                        }

                        shouldProcess = mSyncedActions.Count > 0 && IsTaskProcessingEnabled;
                    }
                    CallTaskAddedOrRemoved();

                    if (shouldProcess)
                    {
                        ProcessNextSync();
                    }

                });
                CallTaskAddedOrRemoved();
            }
        }

        public void OnUiThread(Action action)
        {
#if GLUE
            MainWindow.Self.Invoke(action);
#else
            action();
#endif
        }

        #endregion
    }
}