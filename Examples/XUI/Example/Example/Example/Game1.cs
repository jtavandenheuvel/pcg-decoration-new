//-----------------------------------------------
// XUI - Game1.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

// E_Layer
public enum E_Layer
{
	UI = 0,

	// etc ...

	Count,
};

// class Game1
public class Game1 : Game
{
	// Game1
	public Game1()
		: base()
	{
		Graphics = new GraphicsDeviceManager( this );

		Graphics.PreferredBackBufferWidth = 1280;
		Graphics.PreferredBackBufferHeight = 720;

		Graphics.PreferMultiSampling = true;

	#if PROFILE
		IsFixedTimeStep = false;
		Graphics.SynchronizeWithVerticalRetrace = false;
	#endif

		Content.RootDirectory = "Content";
	}

	// Initialize
	protected override void Initialize()
	{
		_G.Game = this;

		// add core components
		Components.Add( new GamerServicesComponent( this ) );

		// add layers
		UiLayer = new UiLayer();
		_G.UI = UiLayer;

		// add other components
		_G.GameInput = new GameInput( (int)E_GameButton.Count, (int)E_GameAxis.Count );
		GameControls.Setup(); // initialise mappings

		base.Initialize();
	}

	// LoadContent
	protected override void LoadContent()
	{
		Guide.NotificationPosition = NotificationPosition.BottomRight;

		// startup ui
		UiLayer.Startup( Content );

		// setup debug menu
	#if !RELEASE
		_UI.SetupDebugMenu( null );
	#endif

		base.LoadContent();
	}

	// UnloadContent
	protected override void UnloadContent()
	{
		// shutdown ui
		UiLayer.Shutdown();

		base.UnloadContent();
	}

	// Update
	protected override void Update( GameTime gameTime )
	{
		IsRunningSlowly = gameTime.IsRunningSlowly;

		float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

		// update input
		_G.GameInput.Update( frameTime );
		
	#if !RELEASE
		Input input = _G.GameInput.GetInput( 0 );
		
		if ( input.ButtonJustPressed( (int)E_UiButton.Quit ) )
			this.Exit();
	#endif

	#if !RELEASE
		// update debug menu
		_UI.DebugMenuActive = _UI.DebugMenu.Update( frameTime );
	#endif

		// TODO - other stuff here ...

		// update ui
		UiLayer.Update( frameTime );

		base.Update( gameTime );
	}

	// Draw
	protected override void Draw( GameTime gameTime )
	{
		GraphicsDevice.Clear( Color.Black );

		float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

		// TODO - other stuff here ...

		// render ui
		UiLayer.Render( frameTime );

	#if !RELEASE
		// render debug menu
		_UI.DebugMenu.Render();
	#endif

		base.Draw( gameTime );
	}

	//
	private GraphicsDeviceManager		Graphics;
	private UiLayer						UiLayer;

	public  bool						IsRunningSlowly;
	//
};
