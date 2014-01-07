using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

// E_PlayState
public enum E_PlayState
{
	GameWorld = 0,
	LevelEditor,
	TestUI,

	Count,
};

// class UiLayer
public class UiLayer : Layer
{
	// UiLayer
	public UiLayer()
		: base( (int)E_Layer.UI )
	{
		//
	}

	// Startup
	public override void Startup( ContentManager content )
	{
		_UI.Startup( _G.Game, _G.GameInput );

		// load textures
		int bundleIndex = _UI.Texture.CreateBundle();

		_UI.Texture.Add( bundleIndex, "Textures\\UI_Box", "box" );
		_UI.Texture.Add( bundleIndex, "Textures\\UI_Shine", "shine" );
		_UI.Texture.Add( bundleIndex, "Textures\\UI_Buttons", "buttons" );
		_UI.Texture.Add( bundleIndex, "Textures\\UI_FontMult", "font_mult" );
		_UI.Texture.Add( bundleIndex, "Textures\\UI_DebugMenu", "debug_menu" );
		_UI.Texture.Add( bundleIndex, "Textures\\tick", "tick" );

		_UI.Texture.Add( bundleIndex, "Textures\\World_1", "world_1" );
		_UI.Texture.Add( bundleIndex, "Textures\\World_2", "world_2" );
		_UI.Texture.Add( bundleIndex, "Textures\\World_3", "world_3" );
		_UI.Texture.Add( bundleIndex, "Textures\\World_4", "world_4" );

		_UI.Texture.Add( bundleIndex, "Textures\\MM_NewGame", "mm_newgame" );
		_UI.Texture.Add( bundleIndex, "Textures\\MM_LevelSelect", "mm_levelselect" );
		_UI.Texture.Add( bundleIndex, "Textures\\MM_Options", "mm_options" );
		_UI.Texture.Add( bundleIndex, "Textures\\MM_Quit", "mm_quit" );

		_UI.Texture.Add( bundleIndex, "Textures\\CompanyLogo", "company_logo" );
		_UI.Texture.Add( bundleIndex, "Textures\\GameLogo", "game_logo" );

		// load fonts
		_UI.Font.Add( "Fonts\\", "Verdana" );
		_UI.Font.Add( "Fonts\\", "Impact" );

		// everyone has initial control
		_UI.PrimaryPad = -1;

		// setup colors
		_UI.Store_Color.Add( "Blue", 0xff0000ff );
		_UI.Store_Color.Add( "Red", 0xffff0000 );

		// setup timelines
		UI.Timeline timelineText = new UI.Timeline( "selected", false, 0.0f, 0.25f, UI.E_TimerType.Stop, UI.E_RestType.Start );
		timelineText.AddEffect( new UI.TimelineEffect_ColorLerp( Color.Orange, UI.E_LerpType.SmoothStep ) );
		_UI.Store_Timeline.Add( "text_selected_color", timelineText );

		UI.Timeline timeline_StartAlpha = new UI.Timeline( "start", false, 0.0f, 0.25f, UI.E_TimerType.Stop, UI.E_RestType.Start );
		timeline_StartAlpha.AddEffect( new UI.TimelineEffect_Alpha( -1.0f, 0.0f, UI.E_LerpType.SmoothStep ) );
		_UI.Store_Timeline.Add( "text_start_alpha", timeline_StartAlpha );

		// setup textures
		UI.SpriteTexture texture = new UI.SpriteTexture( "buttons", 0.0f, 0.0f, 0.5f, 1.0f );
		_UI.Store_Texture.Add( "buttons_half", texture );

		// setup font styles
		UI.FontStyle fontStyle = new UI.FontStyle( "Impact" );
		fontStyle.AddRenderPass( new UI.FontStyleRenderPass() );
		_UI.Store_FontStyle.Add( "Default", fontStyle );

		UI.FontStyle fontStyleDS = new UI.FontStyle( "Impact" );
		UI.FontStyleRenderPass renderPassDS = new UI.FontStyleRenderPass();
		renderPassDS.ColorOverride = Color.Black;
		renderPassDS.AlphaMult = 0.5f;
		renderPassDS.Offset = new Vector3( 0.05f, -0.05f, 0.0f );
		renderPassDS.OffsetProportional = true;
		fontStyleDS.AddRenderPass( renderPassDS );
		fontStyleDS.AddRenderPass( new UI.FontStyleRenderPass() );
		_UI.Store_FontStyle.Add( "DefaultDS", fontStyleDS );

		// setup font effects
		UI.FontEffect fontEffect = new UI.FontEffect_Scale( 0.0625f, 0.25f, 2.0f, 1.0f, 2.0f, 1.0f, 2.0f, UI.E_LerpType.Sin );
		_UI.Store_FontEffect.Add( "menu_item_scale", fontEffect );

		// setup font icons
		_UI.Store_FontIcon.Add( "A", new UI.FontIcon( _UI.Texture.Get( "buttons" ), 0.0f, 0.0f, ( 64.0f / 820.0f ), 1.0f ) );
		_UI.Store_FontIcon.Add( "B", new UI.FontIcon( _UI.Texture.Get( "buttons" ), ( 63.0f / 820.0f ), 0.0f, ( 64.0f / 820.0f ), 1.0f ) );
		_UI.Store_FontIcon.Add( "X", new UI.FontIcon( _UI.Texture.Get( "buttons" ), ( 126.0f / 820.0f ), 0.0f, ( 64.0f / 820.0f ), 1.0f ) );
		_UI.Store_FontIcon.Add( "Y", new UI.FontIcon( _UI.Texture.Get( "buttons" ), ( 189.0f / 820.0f ), 0.0f, ( 64.0f / 820.0f ), 1.0f ) );

		// setup widgets
		UI.WidgetText text = new UI.WidgetText();
		text.ColorBase = Color.White;
		text.Align = UI.E_Align.TopCentre;
		text.Position = new Vector3( 0.0f, 18.0f, 0.0f );
		text.Size = new Vector3( 0.0f, 3.0f, 0.0f );
		text.Alpha = 0.5f;
		text.FontStyleName = "Default";
		text.AddTimeline( "text_start_alpha" );
		text.RenderPass = 1;
		_UI.Store_Widget.Add( "font_effect_text", text );

		// set initial screen
		PlayState = E_PlayState.TestUI;

		if ( PlayState == E_PlayState.GameWorld )
		{
			_G.Game.DoLayerUpdate( E_Layer.LevelEditor, false );
			_G.Game.DoLayerRender( E_Layer.LevelEditor, false );
			_UI.Screen.AddScreen( new UI.HUD() );
		}
		else
		if ( PlayState == E_PlayState.LevelEditor )
		{
			_G.Game.DoLayerUpdate( E_Layer.GameWorld, false );
			_G.Game.DoLayerRender( E_Layer.GameWorld, false );
			_UI.Screen.AddScreen( new UI.LevelEditorHUD() );
		}
		else
		if ( PlayState == E_PlayState.TestUI )
		{
			_G.Game.DoLayerUpdate( E_Layer.GameWorld, false );
			_G.Game.DoLayerRender( E_Layer.GameWorld, false );
			_G.Game.DoLayerUpdate( E_Layer.LevelEditor, false );
			_G.Game.DoLayerRender( E_Layer.LevelEditor, false );
			_UI.Screen.AddScreen( new UI.Test() );
		}
	}

	// Shutdown
	public override void Shutdown()
	{
		_UI.Shutdown();
	}

	// OnUpdate
	protected override void OnUpdate( float frameTime )
	{
	#if !RELEASE
		if ( _G.Debug.MenuActive )
			return;
	#endif

	#if PROFILE
		using ( new DebugProfileMarker( DebugProfiler.ProfilerUpdateUI, frameTime ) )
		{
	#endif

		_UI.Sprite.BeginUpdate();
		_UI.Screen.Update( frameTime );

	#if !RELEASE
		_UI.Camera2D.DebugUpdate( frameTime, _UI.GameInput.GetInput( 0 ) );
		_UI.Camera3D.DebugUpdate( frameTime, _UI.GameInput.GetInput( 0 ) );
	#endif

	#if PROFILE
		} // profiler update ui
	#endif
	}

	// OnRender
	protected override void OnRender( float frameTime )
	{
	#if PROFILE
		using ( new DebugProfileMarker( DebugProfiler.ProfilerRenderUI, frameTime ) )
		{
	#endif

		_UI.Sprite.TransformMatrix = _UI.Camera2D.TransformMatrix * _UI.Sprite.TransformMatrix2D;
		_UI.Sprite.Render( 2 );

		_UI.Sprite.TransformMatrix = _UI.Camera3D.TransformMatrix;
		_UI.Sprite.Render( 1 );

		_UI.Sprite.TransformMatrix = _UI.Camera2D.TransformMatrix * _UI.Sprite.TransformMatrix2D;
		_UI.Sprite.Render( 0 );

	#if PROFILE
		} // profiler render ui
	#endif
	}

	//
	public  E_PlayState				PlayState		{ get; set; }

	// random globals for screens
	public bool			MM_FromStartScreen		= false;
	public bool			MM_FromLevelSelect		= false;
	public bool			SS_FromMainMenu			= false;
	//
};
