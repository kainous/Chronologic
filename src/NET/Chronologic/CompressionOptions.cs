namespace Chronologic {
    public readonly struct CompressionOptions {
        public byte CompressionLevel { get; }
        public byte ValueLossiness { get; }
        public byte TimeLossiness { get; }

        public CompressionOptions(byte compressionLevel, byte valueLossiness, byte timeLossiness) {
            CompressionLevel = compressionLevel;
            ValueLossiness = valueLossiness;
            TimeLossiness = timeLossiness;
        }
    }
}
