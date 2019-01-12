namespace My.JDownloader.Api.ApiObjects
{
    public class Enums
    {
        public enum PriorityType
        {
            HIGHEST,
            HIGHER,
            HIGH,
            DEFAULT,
            LOW,
            LOWER,
            LOWEST
        }

        public enum ControllerStatus
        {
            //Extraction is currently running
            RUNNING,
            //Archive is queued for extraction and will run as soon as possible
            QUEUED,
            //No controller assigned
            NA
        }

        public enum ArchiveFileStatus
        {
            //  File is available for extraction
            COMPLETE,
            //File exists, but is incomplete
            INCOMPLETE,
            //File does not exist
            MISSING
        }

        public enum Action
        {
            DELETE_ALL,
            DELETE_DISABLED,
            DELETE_FAILED,
            DELETE_FINISHED,
            DELETE_OFFLINE,
            DELETE_DUPE,
            DELETE_MODE
        }

        public enum Mode
        {
            REMOVE_LINKS_AND_DELETE_FILES,
            REMOVE_LINKS_AND_RECYCLE_FILES,
            REMOVE_LINKS_ONLY
        }

        public enum SelectionType
        {
            SELECTED,
            UNSELECTED,
            ALL,
            NONE
        }
    }
}
