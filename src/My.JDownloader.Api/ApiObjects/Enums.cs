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
            // File is available for extraction
            COMPLETE,
            //File exists, but is incomplete
            INCOMPLETE,
            //File does not exist
            MISSING
        }

        public enum Reason
        {

            CONNECTION_UNAVAILABLE,
            TOO_MANY_RETRIES,
            CAPTCHA,
            MANUAL,
            DISK_FULL,
            NO_ACCOUNT,
            INVALID_DESTINATION,
            FILE_EXISTS,
            UPDATE_RESTART_REQUIRED,
            FFMPEG_MISSING,
            FFPROBE_MISSING
        }


        public sealed class Action
        {
            private readonly string value;

            public static readonly Action DELETE_ALL = new Action("DELETE_ALL");
            public static readonly Action DELETE_DISABLED = new Action("DELETE_DISABLED");
            public static readonly Action DELETE_FAILED = new Action("DELETE_FAILED");
            public static readonly Action DELETE_FINISHED = new Action("DELETE_FINISHED");
            public static readonly Action DELETE_OFFLINE = new Action("DELETE_OFFLINE");
            public static readonly Action DELETE_DUPE = new Action("DELETE_DUPE");
            public static readonly Action DELETE_MODE = new Action("DELETE_MODE");

            private Action(string value)
            {
                this.value = value;
            }

            public override string ToString()
            {
                return value;
            }
        }

        public sealed class Mode
        {
            private readonly string value;

            public static readonly Mode REMOVE_LINKS_AND_DELETE_FILES = new Mode("REMOVE_LINKS_AND_DELETE_FILES");
            public static readonly Mode REMOVE_LINKS_AND_RECYCLE_FILES = new Mode("REMOVE_LINKS_AND_RECYCLE_FILES");
            public static readonly Mode REMOVE_LINKS_ONLY = new Mode("REMOVE_LINKS_ONLY");

            private Mode(string value)
            {
                this.value = value;
            }

            public override string ToString()
            {
                return value;
            }
        }

        public sealed class SelectionType
        {

            private readonly string value;
            public static readonly SelectionType SELECTED = new SelectionType("SELECTED");
            public static readonly SelectionType UNSELECTED = new SelectionType("UNSELECTED");
            public static readonly SelectionType ALL = new SelectionType("ALL");
            public static readonly SelectionType NONE = new SelectionType("NONE");

            private SelectionType(string value)
            {
                this.value = value;
            }

            public override string ToString()
            {
                return value;
            }
        }
    }
}
