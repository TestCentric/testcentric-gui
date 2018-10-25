namespace TestCentric.Gui.Model
{
    public delegate void AssemblyChangedHandler(string fullPath);
    public interface IAsemblyWatcher
    {
        /// <summary>
        /// Stops watching for changes.
        /// To release resources call FreeResources.
        /// </summary>
        void Stop();

        /// <summary>
        /// Starts watching for assembly changes.
        /// You need to call Setup before start watching.
        /// </summary>
        void Start();

        /// <summary>
        /// Initializes the watcher with assemblies to observe for changes.
        /// </summary>
        /// <param name="delayInMs">The delay in ms.</param>
        /// <param name="assemblies">The assemblies.</param>
#if CLR_2_0 || CLR_4_0
        void Setup(int delayInMs, System.Collections.Generic.IList<string> assemblies);
#else
        void Setup(int delayInMs, System.Collections.IList assemblies);
#endif

        /// <summary>
        /// Initializes the watcher with assemblies to observe for changes.
        /// </summary>
        /// <param name="delayInMs">The delay in ms.</param>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        void Setup(int delayInMs, string assemblyFileName);

        /// <summary>
        /// Releases all resources held by the watcher.
        /// </summary>
        void FreeResources();

        /// <summary>
        /// Occurs when an assembly being watched has changed.
        /// </summary>
        event AssemblyChangedHandler AssemblyChanged;
    }
}