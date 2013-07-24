namespace XamlHelpmeet.Utility
{
    /// <summary>
    ///     Values that represent NarrowSelectionResult.
    /// </summary>
    public enum NarrowSelectionResult
    {
        /// <summary>
        ///     The current selection is empty.
        /// </summary>
        SelectionIsEmpty,

        /// <summary>
        ///     The end tags were inconsistent.
        /// </summary>
        InconsistentSelectionEnds,

        /// <summary>
        ///     The starting selection was less than a complete tag.
        /// </summary>
        PartialTagSelected,

        /// <summary>
        ///     The narrow selection command succeeded.
        /// </summary>
        Success
    }
}
