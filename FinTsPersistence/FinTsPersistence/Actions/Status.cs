namespace FinTsPersistence.Actions
{
    /// <summary>
    /// Indicates a problem that happened even if all input data was set up correctly 
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// This status should never occur
        /// </summary>
        Unknown,

        /// <summary>
        /// Der Auftrag wurde erfolgreich beendet.
        /// </summary>
        Success,

        /// <summary>
        /// Die 'Dialoginitialisierung' schlug fehl
        /// </summary>
        CouldNotLogOn,

        /// <summary>
        /// The used FinTS api method didn not returned an object of type Subsembly.FinTS.Order
        /// </summary>
        /// <remarks>
        /// Subsembly.FinTS.Order is a base class that holds all segments that participate in a single FinTS order.
        /// </remarks>
        CouldNotCreateOrder,

        /// <summary>
        /// The used ITanSource did not returned a TAN (it returned null)
        /// </summary>
        /// <remarks>Before that the FinService returned FinServiceResult.NeedTan</remarks>
        CouldNotGetTan,

        /// <summary>
        /// Um diesen Auftrag zu senden muss erst eine Bezeichnung des TAN-Mediums in FinContact.Subsembly.FinTS.FinContact.TanMediaName gesetzt werden.
        /// Bei diesem Ergebnis steht normalerweise eine Liste der verfügbaren TAN-Medien in der Eigenschaft Subsembly.FinTS.FinService.TanMedias zur Verfügung.
        /// Nach dem Setzen des TAN-Mediumnamens kann der Auftrag erneut versucht werden.
        /// Dieser Fehlercode kann nur beim Senden von TAN-pflichtigen Aufträgen geliefert werden.
        /// </summary>
        NeedTanMediaName,

        /// <summary>
        /// Es kam zu einem Übertragungsfehler oder einem anderen schwerwiegenden Fehler.
        /// Es ist unklar, ob der Auftrag bei der Bank angekommen ist.
        /// Eine Bankantwort wurde jedenfalls nicht empfangen.
        /// </summary>
        /// <remarks>FinServiceResult.Fatal</remarks>
        FatalResult,

        /// <summary>
        /// Der Auftrag wurde übertragen, aber nicht ausgeführt.
        /// Das Banksystem hat einen Rückmeldecode der Klasse 9000 geliefert, welcher protokolliert wurde.
        /// </summary>
        ErrorResult
    }
}