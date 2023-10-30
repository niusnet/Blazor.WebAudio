﻿using KristofferStrube.Blazor.WebAudio.Extensions;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebAudio;

/// <summary>
/// <see cref="OscillatorNode"/> represents an audio source generating a periodic waveform. It can be set to a few commonly used waveforms. Additionally, it can be set to an arbitrary periodic waveform through the use of a <see cref="PeriodicWave"/> object.
/// </summary>
/// <remarks><see href="https://www.w3.org/TR/webaudio/#OscillatorNode">See the API definition here</see>.</remarks>
public class OscillatorNode : AudioScheduledSourceNode
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of an <see cref="OscillatorNode"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="OscillatorNode"/>.</param>
    /// <returns>A wrapper instance for an <see cref="OscillatorNode"/>.</returns>
    public static new Task<OscillatorNode> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new OscillatorNode(jSRuntime, jSReference));
    }

    /// <summary>
    /// Creates an <see cref="OscillatorNode"/> using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="context">The <see cref="BaseAudioContext"/> this new <see cref="OscillatorNode"/> will be associated with.</param>
    /// <param name="options">Optional initial parameter value for this <see cref="OscillatorNode"/>.</param>
    /// <returns>A new instance of an <see cref="OscillatorNode"/>.</returns>
    public static async Task<OscillatorNode> CreateAsync(IJSRuntime jSRuntime, BaseAudioContext context, OscillatorOptions? options = null)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructOcillatorNode", context.JSReference, options);
        return new OscillatorNode(jSRuntime, jSInstance);
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of an <see cref="OscillatorNode"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="OscillatorNode"/>.</param>
    protected OscillatorNode(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference)
    {
    }

    /// <summary>
    /// The shape of the periodic waveform.
    /// The default value is <see cref="OscillatorType.Sine"/>.
    /// </summary>
    public async Task<OscillatorType> GetTypeAsync()
    {
        IJSObjectReference helper = await webAudioHelperTask.Value;
        return await helper.InvokeAsync<OscillatorType>("getAttribute", JSReference, "type");
    }

    /// <summary>
    /// The shape of the periodic waveform.
    /// The <see cref="SetPeriodicWaveAsync"/> method can be used to set a custom waveform, which results in this attribute being set to <see cref="OscillatorType.Custom"/>.
    /// The default value is <see cref="OscillatorType.Sine"/>.
    /// When this attribute is set, the phase of the oscillator will be conserved.
    /// </summary>
    public async Task SetTypeAsync(OscillatorType value)
    {
        IJSObjectReference helper = await webAudioHelperTask.Value;
        await helper.InvokeVoidAsync("setAttribute", JSReference, "type", value);
    }

    /// <summary>
    /// The frequency (in Hertz) of the periodic waveform.
    /// Its default value is <c>440</c>.
    /// This parameter is <see cref="AutomationRate.ARate"/>.
    /// It forms a compound parameter with detune to form the computedOscFrequency.
    /// Its nominal range is [-Nyquist frequency, Nyquist frequency].
    /// </summary>
    public async Task<AudioParam> GetFrequencyAsync()
    {
        IJSObjectReference helper = await webAudioHelperTask.Value;
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, "frequency");
        return await AudioParam.CreateAsync(JSRuntime, jSInstance);
    }

    /// <summary>
    /// A detuning value (in cents) which will offset the frequency by the given amount.
    /// Its default value is <c>0</c>.
    /// This parameter is <see cref="AutomationRate.ARate"/>.
    /// It forms a compound parameter with frequency to form the computedOscFrequency.
    /// The nominal range listed below allows this parameter to detune the frequency over the entire possible range of frequencies.
    /// </summary>
    public async Task<AudioParam> GetDetuneAsync()
    {
        IJSObjectReference helper = await webAudioHelperTask.Value;
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, "detune");
        return await AudioParam.CreateAsync(JSRuntime, jSInstance);
    }

    /// <summary>
    /// Sets an arbitrary custom periodic waveform given a <see cref="PeriodicWave"/>.
    /// </summary>
    /// <param name="periodicWave">Custom waveform to be used by the oscillator</param>
    /// <returns></returns>
    public async Task SetPeriodicWaveAsync(PeriodicWave periodicWave)
    {
        await JSReference.InvokeVoidAsync("setPeriodicWave", periodicWave.JSReference);
    }
}
